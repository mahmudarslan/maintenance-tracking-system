import { ConfigStateService, CurrentUserDto, LocalizationPipe, LocalizationService } from "@abp/ng.core";
import { Injectable } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { DxButtonComponent } from "devextreme-angular";
import { trigger } from "devextreme/events";
import notify from "devextreme/ui/notify";
import validationEngine from "devextreme/ui/validation_engine";
import { BehaviorSubject, Observable } from "rxjs";
import { NotifyType, SharedButtonModel } from "./models";

@Injectable({
    providedIn: 'root'
})
export class SharedService {
    bClickSendSubject$ = new BehaviorSubject(new SharedButtonModel());
    bComplatedSubject$ = new BehaviorSubject(new SharedButtonModel());
    private localization: LocalizationPipe;

    currentUser: CurrentUserDto = this.configState.getOne("currentUser");

    constructor(
        private router: Router,
        private configState: ConfigStateService,
        private localizationService: LocalizationService) {

        this.localization = new LocalizationPipe(this.localizationService);
    }

    getLocalizationTex(text: string) {
        return this.localization.transform(text);
    }

    alert(message: string, type: NotifyType) {
        notify({
            message: this.localization.transform(message),
            type: NotifyType[type],
            displayTime: 4000,
            height: 40,
            width: 3000,
            position: {
                my: "right",
                at: "right",
                of: "#kt_header_user_menu_toggle"
            }
        });
    }

    notifySuccess(message: string) {
        this.alert(message, NotifyType.success)
    }

    notifyInfo(message: string) {
        this.alert(message, NotifyType.info)
    }

    notifyWarning(message: string) {
        this.alert(message, NotifyType.warning)
    }

    notifyError(message: string) {
        this.alert(message, NotifyType.error)
    }

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