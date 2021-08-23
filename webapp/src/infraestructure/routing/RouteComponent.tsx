import React, { ReactComponentElement, ReactNode } from 'react';
import { Resource } from 'infraestructure/Resource';
import { PreparedMatch } from 'infraestructure/routing/Router';
import { match } from 'react-router';

/**
 * The `component` property from the route entry is a Resource, which may or may not be ready.
 * We use a helper child component to unwrap the resource with component.read(), and then
 * render it if its ready.
 *
 * NOTE: calling routeEntry.route.component.read() directly in RouteRenderer wouldn't work the
 * way we'd expect. Because that method could throw - either suspending or on error - the error
 * would bubble up to the *caller* of RouteRenderer. We want the suspend/error to bubble up to
 * our ErrorBoundary/Suspense components, so we have to ensure that the suspend/error happens
 * in a child component.
 */
interface RouteComponentProps {
  component: Resource<React.ComponentType<any>>;
  children?: ReactNode;
  routeData: match<any>;
  prepared?: unknown;
}

const RouteComponent = (props: RouteComponentProps) => {
  const Component = props.component?.read();
  const { routeData, prepared, children } = props;

  return (
    <Component routeData={routeData} prepared={prepared} children={children} />
  );
};

export default RouteComponent;
