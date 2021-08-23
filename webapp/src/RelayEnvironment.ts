import {
  Environment,
  Network,
  RecordSource,
  RequestParameters,
  Store,
  GraphQLResponse,
} from 'relay-runtime';
import {
  CacheConfig,
  Variables,
} from 'relay-runtime/lib/util/RelayRuntimeTypes';
import { UploadableMap } from 'relay-runtime/lib/network/RelayNetworkTypes';

/**
 * Relay requires developers to configure a "fetch" function that tells Relay how to load
 * the results of GraphQL queries from your server (or other data source). See more at
 * https://relay.dev/docs/en/quick-start-guide#relay-environment.
 */
async function fetchRelay(
  params: RequestParameters,
  variables: Variables,
  // eslint-disable-next-line @typescript-eslint/no-unused-vars
  _cacheConfig: CacheConfig,
  // eslint-disable-next-line @typescript-eslint/no-unused-vars
  _?: UploadableMap | null,
): Promise<GraphQLResponse> {
  // Check that the auth token is configured
  // const REACT_APP_GITHUB_AUTH_TOKEN = process.env.REACT_APP_GITHUB_AUTH_TOKEN;
  // if (
  //   REACT_APP_GITHUB_AUTH_TOKEN == null ||
  //   REACT_APP_GITHUB_AUTH_TOKEN === ''
  // ) {
  //   throw new Error(
  //     'This app requires a GitHub authentication token to be configured. See readme.md for setup details.',
  //   );
  // }

  // Fetch data from GitHub's GraphQL API:
  const response: Response = await fetch('http://localhost:5000/graphql', {
    method: 'POST',
    headers: {
      // Authorization: `bearer ${REACT_APP_GITHUB_AUTH_TOKEN}`,
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({
      query: params.text,
      variables,
    }),
  });

  // TODO Improve error handling
  let graphqlResponse = (await response.json()) as GraphQLResponse;

  // GraphQL returns exceptions (for example, a missing required variable) in the "errors"
  // property of the response. If any exceptions occurred when processing the request,
  // throw an error to indicate to the developer what went wrong.
  graphqlResponse = !Array.isArray(graphqlResponse)
    ? [graphqlResponse]
    : graphqlResponse;

  for (const element of graphqlResponse) {
    if ('errors' in element) {
      console.log(element.errors);
      throw new Error(`Error fetching GraphQL query '${params.name}' with 
            variables '${JSON.stringify(variables)}': ${JSON.stringify(
        element.errors,
      )}`);
    }
  }

  // Otherwise, return the full payload.
  return graphqlResponse;
}

// Export a singleton instance of Relay Environment configured with our network layer:
export default new Environment({
  network: Network.create(fetchRelay),
  store: new Store(new RecordSource(), {
    // This property tells Relay to not immediately clear its cache when the user
    // navigates around the app. Relay will hold onto the specified number of
    // query results, allowing the user to return to recently visited pages
    // and reusing cached data if its available/fresh.
    gcReleaseBufferSize: 10,
  }),
});
