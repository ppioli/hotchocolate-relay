import React, { ReactNode } from 'react';

/**
 * A reusable component for handling errors in a React (sub)tree.
 */
export default class ErrorBoundary extends React.Component {
  state: { error: Error | null };

  constructor(props: { children: ReactNode }) {
    super(props);
    this.state = { error: null };
  }

  static getDerivedStateFromError(error: Error) {
    return {
      error,
    };
  }

  render() {
    if (this.state.error != null) {
      return (
        <div>
          <div>Error: {this.state.error.message}</div>
          <div>
            <pre>{JSON.stringify(this.state.error, null, 2)}</pre>
          </div>
        </div>
      );
    }
    return this.props.children;
  }
}
