import { loadQuery } from 'react-relay/hooks';
import RelayEnvironment from 'RelayEnvironment';
import JSResource from './infraestructure/JSResource';
import { RelayRouteConfig } from 'infraestructure/routing/RelayRouteConfig';
import AccountsQuery from '__generated__/AccountsPageQuery.graphql';
import AccountDetailQuery from '__generated__/AccountDetailQuery.graphql';
import React from 'react';
import { AppProps } from 'App';

const routes: RelayRouteConfig[] = [
  {
    lazyComponent: JSResource<React.ComponentType<AppProps>>(
      'App',
      () => import('./App'),
    ),
    routes: [
      {
        path: '/',
        exact: true,
        /**
         * A lazy reference to the component for the home route. Note that we intentionally don't
         * use React.lazy here: that would start loading the component only when it's rendered.
         * By using a custom alternative we can start loading the code instantly. This is
         * especially useful with nested routes, where React.lazy would not fetch the
         * component until its parents code/data had loaded.
         */
        lazyComponent: JSResource(
          'AccountsPage',
          () => import('features/account/AccountsPage'),
        ),
        /**
         * A function to prepare the data for the `component` in parallel with loading
         * that component code. The actual data to fetch is defined by the component
         * itself - here we just reference a description of the data - the generated
         * query.
         */
        prepare: () => {
          console.log('query', AccountsQuery);
          return {
            accountsQuery: loadQuery(
              RelayEnvironment,
              AccountsQuery,
              {},
              // The fetchPolicy allows us to specify whether to render from cached
              // data if possible (store-or-network) or only fetch from network
              // (network-only).
              { fetchPolicy: 'store-or-network' },
            ),
          };
        },
      },
      {
        path: '/account/:id',
        lazyComponent: JSResource(
          'AccountDetail',
          () => import('features/account/AccountDetail'),
        ),
        prepare: (params: Record<string, unknown>) => {
          return {
            query: loadQuery(
              RelayEnvironment,
              AccountDetailQuery,
              {
                id: params.id,
              },
              { fetchPolicy: 'store-or-network' },
            ),
          };
        },
      },
    ],
  },
];

export default routes;
