import { eLayoutType, RoutesService } from '@abp/ng.core';
import { APP_INITIALIZER } from '@angular/core';
import { ePlannerPolicyNames } from '../enums/policy-names';
import { ePlannerRouteNames } from '../enums/route-names';

export const PLANNER_ROUTE_PROVIDERS = [
  { provide: APP_INITIALIZER, useFactory: configureRoutes, deps: [RoutesService], multi: true },
];

export function configureRoutes(routesService: RoutesService) {
  return () => {
    routesService.add([
      {
        path: '/planner',
        name: ePlannerRouteNames.Planner,
        requiredPolicy: ePlannerPolicyNames.Scheduler,
        iconClass: 'browser-outline',
        layout: eLayoutType.application,
        order: 60,
      },
      {
        path: '/planner/scheduler/scheduler',
        name: ePlannerRouteNames.Scheduler,
        parentName: ePlannerRouteNames.Planner,
        requiredPolicy: ePlannerPolicyNames.Scheduler,
        iconClass: 'browser-outline',
        layout: eLayoutType.application,
        order: 1,
      },
    ]);
  };
}
