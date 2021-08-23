import graphql from 'babel-plugin-relay/macro';
import { PreloadedQuery, usePreloadedQuery } from 'react-relay';
import AccountList from './AccountList';
import { AccountsPageQuery } from '__generated__/AccountsPageQuery.graphql';

export interface AccountsPageProps {
  prepared: { accountsQuery: PreloadedQuery<AccountsPageQuery> };
}

const AccountsPage = (props: AccountsPageProps) => {
  console.log(props);
  const data = usePreloadedQuery(
    graphql`
      query AccountsPageQuery {
        ...AccountList_account
      }
    `,
    props.prepared.accountsQuery,
  );

  return <AccountList accounts={data} />;
};

export default AccountsPage;
