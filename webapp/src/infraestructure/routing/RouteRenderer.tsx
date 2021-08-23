import React from 'react';
import ErrorBoundary from '../ErrorBoundary';
import './RouteRenderer.css';
import RouterContext from './RouterContext';
import RouteComponent from './RouteComponent';
import { LoaderType, Resource, wrapResource } from 'infraestructure/Resource';

const { useContext, useEffect, Suspense, useState } = React;

/**
 * A component that accesses the current route entry from RoutingContext and renders
 * that entry.
 */
const RouterRenderer: React.ComponentType = () => {
  // Access the router
  const router = useContext(RouterContext);

  // Store the active entry in state - this allows the renderer to use features like
  // useTransition to delay when state changes become visible to the user.
  const [routeEntry, setRouteEntry] = useState(router.get());

  // On mount subscribe for route changes
  useEffect(() => {
    // Check if the route has changed between the last render and commit:
    const currentEntry = router.get();
    if (currentEntry !== routeEntry) {
      // if there was a concurrent modification, rerender and exit
      setRouteEntry(currentEntry);
      return;
    }

    // If there *wasn't* a concurrent change to the route, then the UI
    // is current: subscribe for subsequent route updates
    const dispose = router.subscribe((nextEntry) => {
      // startTransition() delays the effect of the setRouteEntry (setState) call
      // for a brief period, continuing to show the old state while the new
      // state (route) is prepared.
      // startTransition(() => {
      setRouteEntry(nextEntry);
      // });
    });
    return () => dispose();

    // Note: this hook updates routeEntry manually; we exclude that variable
    // from the hook deps to avoid recomputing the effect after each change
    // triggered by the effect itself.
    // eslint-disable-next-line
  }, [router]);

  // The current route value is an array of matching entries - one entry per
  // level of routes (to allow nested routes). We have to map each one to a
  // RouteComponent to allow suspending, and also pass its children correctly.
  // Conceptually, we want this structure:
  // ```
  // <RouteComponent
  //   component={entry[0].component}
  //   prepared={entrry[0].prepared}>
  //   <RouteComponent
  //     component={entry[1].component}
  //     prepared={entry[1].prepared}>
  //       // continue for nested items...
  //   </RouteComponent>
  // </RouteComponent>
  // ```
  // To achieve this, we reverse the list so we can start at the bottom-most
  // component, and iteratively construct parent components w the previous
  // value as the child of the next one:
  const reversedItems = [...routeEntry.entries].reverse(); // reverse is in place, but we want a copy so concat
  const firstItem = reversedItems[0];
  // the bottom-most component is special since it will have no children
  // (though we could probably just pass null children to it)
  let routeComponent = (
    <RouteComponent
      component={
        firstItem.component ?? wrapResource(firstItem.route.component!)
      }
      prepared={firstItem.prepared as unknown}
      routeData={firstItem.match}
    />
  );
  for (let ii = 1; ii < reversedItems.length; ii++) {
    const nextItem = reversedItems[ii];
    routeComponent = (
      <RouteComponent
        component={
          nextItem.component ?? wrapResource(nextItem.route.component!)
        }
        prepared={nextItem.prepared as unknown}
        routeData={nextItem.match}
      >
        {routeComponent}
      </RouteComponent>
    );
  }

  // Routes can error so wrap in an <ErrorBoundary>
  // Routes can suspend, so wrap in <Suspense>
  return (
    <ErrorBoundary>
      <Suspense fallback={'Loading fallback...'}>
        {/* Indicate to the user that a transition is pending, even while showing the previous UI */}
        {/*{isPending ? (*/}
        {/*  <div className="RouteRenderer-pending">Loading pending...</div>*/}
        {/*) : null}*/}
        {routeComponent}
      </Suspense>
    </ErrorBoundary>
  );
};

export default RouterRenderer;
