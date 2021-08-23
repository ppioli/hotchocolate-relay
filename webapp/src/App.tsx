import React, { FunctionComponent, ReactNode } from 'react';
import { RouteComponentProps } from 'react-router';

const { Suspense } = React;

export interface AppProps {
  title?: string;
}

const App: React.ComponentType<AppProps> = ({ title, children }) => {
  // Defines *what* data the component needs via a query. The responsibility of
  // actually fetching this data belongs to the route definition: it calls
  // preloadQuery() with the query and variables, and the result is passed
  // on props.prepared.issuesQuery - see src/routes.ts
  // const data = usePreloadedQuery(
  //     graphql`
  //         query AppQuery {
  //
  //         }
  //     `,
  //     props.prepared.rootQuery,
  // );
  // const { repository } = data;

  return (
    <div className="root">
      <header className="header">{title}</header>
      <section className="content">
        {/* Wrap the child in a Suspense boundary to allow rendering the
        layout even if the main content isn't ready */}
        <Suspense fallback={'Loading...'}>{children}</Suspense>
      </section>
    </div>
  );
};

export default App;
