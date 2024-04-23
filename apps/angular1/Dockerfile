FROM node:18-alpine as build

WORKDIR /app
RUN apk update && npm install -g @angular/cli
COPY ./apps/angular1/package*.json /app/

RUN npm install
COPY ./apps/angular1/ /app
ARG configuration=production
RUN npm run ng build -- --output-path=dist/ArslanVms --configuration $configuration

FROM nginx:alpine
COPY /apps/angular1/dynamic-env.json /usr/share/nginx/html
COPY /apps/angular1/nginx.conf  /etc/nginx/conf.d/default.conf

COPY --from=build /app/dist/ArslanVms/ /usr/share/nginx/html
