import { ApiInterceptor } from "@abp/ng.core";
import * as AspNetData from "devextreme-aspnet-data-nojquery";
import { Injectable } from "@angular/core";
import CustomStore from "devextreme/data/custom_store";
import { Observable } from "rxjs";
import { HttpClient, HttpHeaders, HttpParams } from "@angular/common/http";
import { map } from 'rxjs/operators';
import { environment } from "../../../environments/environment";

@Injectable({
    providedIn: 'root'
})
export class HelperService {
    constructor(
        private http: HttpClient,
        private apiInterceptor: ApiInterceptor) {
    }

    getStorageFilterValue?(storageName: string, filterName: string): boolean {
        let returnValue;
        let values = JSON.parse(localStorage.getItem(storageName));
        if (values && values.filterValue) {
            returnValue = null;
            for (let i = 0; i < values.filterValue.length; i++) {
                if (Array.isArray(values.filterValue[i])) {
                    let value = values.filterValue[i];
                    if (value[0] == filterName) {
                        returnValue = value[2];
                        break;
                    }
                } else {
                    if (values.filterValue[i] == filterName) {
                        returnValue = values.filterValue[i + 2];
                        break;
                    }
                }
            }
        } else {
            if (!values) {
                returnValue = false;
            } else {
                returnValue = null;
            }
        }

        return returnValue;
    }

    gridDatasourceBind(url: string, key: string = "id", loadUrl: string = ""): CustomStore {
        url = environment.apis.default.url + url;
        let headers = this.apiInterceptor.getAdditionalHeaders();

        return AspNetData.createStore({
            key: key,
            loadUrl: url + "/" + loadUrl,
            insertUrl: url + "/",
            updateUrl: url + "/",
            deleteUrl: url + "/",
            onBeforeSend: function (method, ajaxOptions) {
                //ajaxOptions.xhrFields = { withCredentials: true };
                ajaxOptions.headers = headers;
            },
            onLoading: (loadOptions) => {
                //console.log(loadOptions.filter)
            },
            onLoaded: (result) => {
                //console.log(result.length);
            },

            onInserting: (values) => {
                //console.log(values)
            },
            onInserted: (values, key) => {
                //console.log(values, key)
            },

            onUpdating: (key, values) => {
                //console.log(key, values)
            },
            onUpdated: (key, values) => {
                //console.log(key, values)
            },

            onRemoving: (key) => {
                //console.log(key)
            },
            onRemoved: (key) => {
                //console.log(key)
            },

            onModifying: () => {
                //console.log("modifying")
            },
            onModified: () => {
                //console.log("modified")
            },

            onPush: (changes) => {
                //console.log(changes.length)
            }
        });
    }

    download(salesorder: string): Observable<any> {
        let url = environment.apis.default.url + `rest/api/latest/vms/sales/download?workorderNo=${salesorder}`;
        let headers = this.apiInterceptor.getAdditionalHeaders();

        return this.http.get(url, { responseType: 'arraybuffer', headers: headers }
        ).pipe(map(response => {
            return this.downLoadFile(response, "application/pdf")
        }));

    }

    download1(id: string, fileName: string, extention: string) {
        let url = environment.apis.default.url + 'rest/api/latest/vms/base/file/download?id=' + id;
        let headers = new HttpHeaders();

        this.http.get(url, { responseType: 'arraybuffer', headers: headers }
        ).subscribe(response => {
            let data: any;
            data = response;
            const blob = new Blob([data], { type: "application/" + extention });
            const url = window.URL.createObjectURL(blob);
            const anchor = document.createElement("a");
            anchor.download = fileName + "." + extention;
            anchor.href = url;
            anchor.click();
        }
        );
    }

    private downLoadFile(data: any, type: string) {
        let blob = new Blob([data], { type: type });
        let url = window.URL.createObjectURL(blob);
        let pwa = window.open(url);
        if (!pwa || pwa.closed || typeof pwa.closed == 'undefined') {
            alert('Please disable your Pop-up blocker and try again.');
        }
    }
};