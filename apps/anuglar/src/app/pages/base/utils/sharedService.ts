import { LocalizationService } from "@abp/ng.core";
import { Injectable } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { DxButtonComponent } from "devextreme-angular";
import { trigger } from "devextreme/events";
import notify from "devextreme/ui/notify";
import validationEngine from "devextreme/ui/validation_engine";
import { BehaviorSubject, Observable } from "rxjs";
import { SharedButtonModel } from "../models/model";

@Injectable({
    providedIn: 'root'
})
export class SharedService {
    constructor(
        private router: Router,
        private localizationService: LocalizationService) {
    }

    bClickSendSubject$ = new BehaviorSubject(new SharedButtonModel());
    bComplatedSubject$ = new BehaviorSubject(new SharedButtonModel());


    // buttonSubscribe(button: SharedButtonModel, model: any): any {
    //     return this.bClickSendSubject$.subscribe(s => {
    //         if (s && !s.name) return;
    //         validationEngine.validateModel(model);

    //         validationEngine.validateGroup();
    //         if (!validationEngine.validateGroup().isValid) {
    //             this.bComplatedSubject$.next(button);
    //         } else {
    //             // return true;
    //             //this.Save();
    //         }
    //     });
    // }

    buttonComplateWithClearTextNotify(bModel: SharedButtonModel, message: string, type: string, navigate?: string) {
        this.bComplatedSubject$.next(bModel);

        notify({
            message: message,
            type: type,
            displayTime: 4000,
            height: 40,
            width: 3000
        });
        if (navigate && type != "error") {
            this.router.navigate([navigate]);
        }
    }

    buttonComplate(bModel: SharedButtonModel) {
        this.bComplatedSubject$.next(bModel);
    }

    buttonComplateWithNotify(bModel: SharedButtonModel, message: string, type: string, navigate?: string) {
        this.bComplatedSubject$.next(bModel);

        if (type == "error") {
            message = this.getErrorMessage(message);
        }

        notify({
            message: message,
            type: type,
            displayTime: 4000,
            height: 40,
            width: 3000
        });
        if (navigate && type != "error") {
            this.router.navigate([navigate]);
        }
    }

    buttonComplateWithLocalizationNotify(bModel: SharedButtonModel, message: string, type: string, navigate?: string) {
        this.bComplatedSubject$.next(bModel);
        this.localizationService.get(message).subscribe(s => {
            notify({
                message: s,
                type: type,
                displayTime: 4000,
                height: 40,
                width: 3000
            });
        });
        if (navigate && type != "error") {
            this.router.navigate([navigate]);
        }
    }

    getErrorMessage(error: any) {
        return (error.message) ? error.message : error.status ? `${error.status} - ${error.statusText}` : 'Server error'
    }

    hideButton(): SharedButtonModel {
        let sharedButton = new SharedButtonModel();
        sharedButton.visible = false;
        sharedButton.hitCount = 0;
        this.bComplatedSubject$.next(sharedButton);
        return sharedButton;
    }

    showButton(isEditMode: boolean): SharedButtonModel {
        let sharedButton = new SharedButtonModel();
        sharedButton.visible = true;
        sharedButton.isEditMode = isEditMode;
        this.bComplatedSubject$.next(sharedButton);
        return sharedButton;
    }
};