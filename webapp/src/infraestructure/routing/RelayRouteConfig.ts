import { Resource } from 'infraestructure/Resource';
import { RouteConfig, RouteConfigComponentProps } from 'react-router-config';
import * as React from 'react';
import { Location } from 'history';

export interface RelayRouteConfig extends RouteConfig {
  key?: React.Key | undefined;
  location?: Location | undefined;
  component?:
    | React.ComponentType<RouteConfigComponentProps<any>>
    | React.ComponentType
    | undefined;
  path?: string | string[] | undefined;
  exact?: boolean | undefined;
  strict?: boolean | undefined;
  routes?: RouteConfig[] | undefined;
  render?:
    | ((props: RouteConfigComponentProps<any>) => React.ReactNode)
    | undefined;
  [propName: string]: unknown;
  lazyComponent?: Resource<React.ComponentType<any>> | undefined;
  prepare?: (params: any) => any | undefined;
}
