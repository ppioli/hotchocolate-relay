import {
  BrowserHistory,
  BrowserHistoryOptions,
  createBrowserHistory,
  Location,
} from 'history';
import { RelayRouteConfig } from 'infraestructure/routing/RelayRouteConfig';
import { MatchedRoute, matchRoutes } from 'react-router-config';
import { Resource } from 'infraestructure/Resource';
import React from 'react';
import { ParamsType } from 'infraestructure/routing/ParamType';
import { match } from 'react-router';

interface Entry {
  location: Location;
  entries: PreparedMatch<any>[];
}
/**
 * A custom router built from the same primitives as react-router. Each object in `routes`
 * contains both a Component and a prepare() function that can preload data for the component.
 * The router watches for changes to the current location via the `history` package, maps the
 * location to the corresponding route entry, and then preloads the code and data for the route.
 */
export default class Router {
  private _routes: RelayRouteConfig[];
  private _currentEntry: Entry;
  private _currentId = 0;
  private _subscribers: Map<number, (entry: Entry) => void>;

  // cleanup function
  public cleanup: () => void;
  public history: BrowserHistory;

  constructor(routes: RelayRouteConfig[], options?: BrowserHistoryOptions) {
    // Initialize history
    this._routes = routes;
    this.history = createBrowserHistory(options);
    this._subscribers = new Map<number, () => void>();
    // Find the initial match and prepare it
    const initialMatches = matchRoute(routes, this.history.location.pathname);
    const initialEntries = prepareMatches(initialMatches);

    this._currentEntry = {
      location: this.history.location,
      entries: initialEntries,
    };

    // Listen for location changes, match to the route entry, prepare the entry,
    // and notify subscribers. Note that this pattern ensures that data-loading
    // occurs *outside* of - and *before* - rendering.
    this.cleanup = this.history.listen(({ location }) => {
      if (location.pathname === this._currentEntry.location.pathname) {
        return;
      }
      const matches = matchRoute(routes, location.pathname);
      const entries = prepareMatches(matches);
      const nextEntry = {
        location,
        entries,
      };
      this._currentEntry = nextEntry;
      this._subscribers.forEach((cb) => cb(nextEntry));
    });
  }

  public get(): Entry {
    return this._currentEntry;
  }

  public preloadCode(pathname: string): void {
    // preload just the code for a route, without storing the result
    const matches = matchRoute(this._routes, pathname);

    // load the lazy component if it exist
    matches.forEach(({ route }) => {
      void route.lazyComponent?.load();
    });
  }

  public preload(pathname: string): void {
    // preload the code and data for a route, without storing the result
    const matches = matchRoute(this._routes, pathname);
    prepareMatches(matches);
  }

  public subscribe(callback: (entry: Entry) => void): () => void {
    const id = this._currentId++;

    const dispose = () => {
      this._subscribers.delete(id);
    };

    this._subscribers.set(id, callback);

    return dispose;
  }
}

/**
 * Match the current location to the corresponding route entry.
 */
function matchRoute<Params extends ParamsType>(
  routes: RelayRouteConfig[],
  pathname: string,
): MatchedRoute<Params, RelayRouteConfig>[] {
  const matchedRoutes = matchRoutes<Params, RelayRouteConfig>(routes, pathname);
  if (!Array.isArray(matchedRoutes) || matchedRoutes.length === 0) {
    throw new Error('No route for ' + pathname);
  }
  console.log(matchedRoutes);
  return matchedRoutes;
}

export interface PreparedMatch<Params extends { [K in keyof Params]?: string }>
  extends MatchedRoute<Params, RelayRouteConfig> {
  route: RelayRouteConfig;
  match: match<Params>;
  prepared?: any;
  component: Resource<React.ComponentType<any>> | undefined;
}
/**
 * Load the data for the matched route, given the params extracted from the route
 */
function prepareMatches<ParamsType>(
  matches: MatchedRoute<ParamsType, RelayRouteConfig>[],
): PreparedMatch<any>[] {
  const result: PreparedMatch<any>[] = matches.map((matchData) => {
    const { route, match } = matchData;
    const prepared: unknown = route.prepare?.(match.params);

    if (route.lazyComponent) {
      const component = route.lazyComponent.get();
      if (component == null) {
        void route.lazyComponent?.load(); // eagerly load
      }
    }

    return {
      component: route.lazyComponent,
      prepared,
      match,
      route,
    };
  });

  return result;
}
