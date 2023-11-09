FROM node:20-alpine AS development 

WORKDIR /app

COPY bazartwebapp/package.json .
COPY bazartwebapp/package-lock.json .

RUN npm ci

COPY bazartwebapp/. .

CMD ["npm", "start"]