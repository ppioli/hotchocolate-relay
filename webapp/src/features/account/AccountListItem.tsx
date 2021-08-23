import graphql from 'babel-plugin-relay/macro';
import { useFragment } from 'react-relay';
import { CSSProperties } from 'react';
import { AccountListItemFragment$key } from '__generated__/AccountListItemFragment.graphql';
import Link from 'infraestructure/routing/Link';

interface AccountItemProps {
  index: number;
  style: CSSProperties;
  data: ReadonlyArray<{ node: AccountListItemFragment$key }>;
}

const AccountListItem = ({ data, index, style }: AccountItemProps) => {
  const AccountItemFragment = graphql`
    fragment AccountListItemFragment on Account {
      id
      name
      isCategory
      code
    }
  `;
  const account = useFragment(AccountItemFragment, data[index].node);
  return (
    <li style={style}>
      {account.name}
      {account.isCategory}
      <Link to={`/account/${account.id}`}>Editar</Link>
    </li>
  );
};

export default AccountListItem;
