import { PreloadedQuery, usePreloadedQuery } from 'react-relay';
import graphql from 'babel-plugin-relay/macro';
import { AccountDetailQuery } from '__generated__/AccountDetailQuery.graphql';

const query = graphql`
  query AccountDetailQuery($id: ID!) {
    node(id: $id) {
      ... on Account {
        code
        name
      }
    }
  }
`;

interface AccountDetailProps {
  prepared: { query: PreloadedQuery<AccountDetailQuery> };
}

const AccountDetail = (props: AccountDetailProps) => {
  const data = usePreloadedQuery(query, props.prepared.query);
  console.log('AccountDetail', data);
  const { code, name } = data.node ?? {};

  return (
    <div>
      {code} - {name}
      <button>Edit</button>
    </div>
  );
};

export default AccountDetail;
