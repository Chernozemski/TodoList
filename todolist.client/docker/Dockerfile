FROM node:21-alpine

WORKDIR /app

COPY ["todolist.client/package.json", "."]
COPY ["todolist.client/tsconfig*.json", "."]
RUN npm install --omit=dev
RUN npm install typescript

COPY . .
WORKDIR "/app/todolist.client"

ENTRYPOINT ["npm", "run", "dev"]