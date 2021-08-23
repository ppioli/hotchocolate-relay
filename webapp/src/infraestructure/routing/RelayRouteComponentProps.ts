import { RouteComponentProps, StaticContext, match } from 'react-router';
import * as H from 'history';
import * as History from 'history';
import { ParamsType } from 'infraestructure/routing/ParamType';

export default interface RelayRouteComponentProps<ParamsType>
  extends RouteComponentProps<ParamsType> {
  history: H.History;
  location: H.Location;
  staticContext?: StaticContext;
  match: match<ParamsType>;
  prepared: any;
  component: React.ComponentType<any>;
}
