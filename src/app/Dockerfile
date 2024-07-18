FROM node:lts-alpine as build
WORKDIR /usr/local/pdf-processor/
COPY . .
RUN npm i
RUN npm run build

FROM nginx:latest
COPY --from=build /usr/local/pdf-processor/dist/pdf-processor/browser /usr/share/nginx/html
EXPOSE 80
