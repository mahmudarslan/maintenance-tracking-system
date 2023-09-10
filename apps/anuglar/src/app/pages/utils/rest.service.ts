import { ABP, CORE_OPTIONS, EnvironmentService, HttpErrorReporterService, isUndefinedOrEmptyString, Rest } from '@abp/ng.core';
import { HttpClient, HttpParameterCodec, HttpParams, HttpRequest } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class CustomRestService {
  constructor(
    @Inject(CORE_OPTIONS) protected options: ABP.Root,
    protected http: HttpClient,
    protected environment: EnvironmentService,
    protected httpErrorReporter: HttpErrorReporterService,
  ) {}

  protected getApiFromStore(apiName: string): string {
    return this.environment.getApiUrl(apiName);
  }

  handleError(err: any): Observable<any> {
    this.httpErrorReporter.reportError(err);
    return throwError(err);
  }
  
  requestList<T, R>(
    request: HttpRequest<T> | Rest.Request<T>,
    loadOptions: any,
    config?: Rest.Config,
    api?: string,    
  ): Observable<R> {

    config = config || ({} as Rest.Config);
    api = api || this.getApiFromStore(config.apiName);
    const { method, ...options } = request;
    const { observe = Rest.Observe.Body, skipHandleError } = config;

    function isNotEmpty(value: any): boolean {
      return value !== undefined && value !== null && value !== "";
    }

    let params: HttpParams = new HttpParams();
    [
      "skip",
      "take",
      "requireTotalCount",
      "requireGroupCount",
      "sort",
      "filter",
      "totalSummary",
      "group",
      "groupSummary"
    ].forEach(function (i) {
      if (i in loadOptions && isNotEmpty(loadOptions[i]))
        params = params.set(i, JSON.stringify(loadOptions[i]));
    });

    return this.http
      .request<R>(method, api + request.url, {
        observe,
        ...(params && {
          params: params,
        }),
        ...options,
      } as any)
      .pipe(catchError(err => (skipHandleError ? throwError(err) : this.handleError(err))));
  }

}
