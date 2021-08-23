import React, { Suspense } from 'react';
import { usePaginationFragment } from 'react-relay';
import graphql from 'babel-plugin-relay/macro';
import AccountListItem from './AccountListItem';
import { FixedSizeList as List } from 'react-window';
import { AccountList_account$key } from '../../__generated__/AccountList_account.graphql';

const fragment = graphql`
  fragment AccountList_account on Query
  @refetchable(queryName: "AccountListPaginationQuery")
  @argumentDefinitions(
    count: { type: "Int" }
    cursor: { type: "String", defaultValue: null }
  ) {
    accounts(first: $count, after: $cursor)
      @connection(key: "AccountList_accounts") {
      edges {
        node {
          ...AccountListItemFragment
        }
      }
    }
  }
`;

interface AccountListProps {
  readonly accounts: AccountList_account$key;
}

const AccountList = (props: AccountListProps) => {
  console.log('AccountList');
  console.log(props);
  const { data, loadNext } = usePaginationFragment(fragment, props.accounts);
  console.log(data);
  return (
    <>
      <Suspense fallback={<li>Loading...</li>}>
        <List
          height={150}
          itemData={data?.accounts?.edges ?? []}
          itemCount={data?.accounts?.edges?.length ?? 0}
          itemSize={50}
          width={300}
        >
          {AccountListItem}
        </List>
      </Suspense>
      <button
        onClick={() => {
          loadNext(10);
        }}
      >
        Load more
      </button>
    </>
  );
};

export default AccountList;
