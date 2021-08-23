import React from 'react';

//TODO check this types
export type LoaderType<T> = () => Promise<DynamicImportType<T>>;
interface DynamicImportType<T> {
  default: T;
}
/**
 * A generic resource: given some method to asynchronously load a value - the loader()
 * argument - it allows accessing the state of the resource.
 */
export class Resource<TResource> {
  private _loader: LoaderType<TResource>;
  private _error: Error | null;
  private _promise: Promise<DynamicImportType<TResource>> | null;
  private _result: TResource | null;

  constructor(loader: LoaderType<TResource>) {
    this._error = null;
    this._loader = loader;
    this._promise = null;
    this._result = null;
  }

  /**
   * Loads the resource if necessary.
   */
  load(): Promise<DynamicImportType<TResource>> {
    if (this._promise == null) {
      this._promise = this._loader()
        .then((result) => {
          this._result = result.default;
          return result;
        })
        .catch((error: Error) => {
          this._error = error;
          throw error;
        });
    }
    return this._promise;
  }

  /**
   * Returns the result, if available. This can be useful to check if the value
   * is resolved yet.
   */
  get(): TResource | null {
    if (this._result != null) {
      return this._result;
    }
    return null;
  }

  /**
   * This is the key method for integrating with React Suspense. Read will:
   * - "Suspend" if the resource is still pending (currently implemented as
   *   throwing a Promise, though this is subject to change in future
   *   versions of React)
   * - Throw an error if the resource failed to load.
   * - Return the data of the resource if available.
   */
  read(): TResource {
    if (this._result != null) {
      return this._result;
    } else if (this._error != null) {
      throw this._error;
    } else {
      throw this._promise;
    }
  }
}

export const wrapResource = <T>(resource: T) => {
  return new Resource(() => Promise.resolve({ default: resource }));
};
