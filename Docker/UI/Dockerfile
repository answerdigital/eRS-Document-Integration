FROM node:latest
WORKDIR /App

# Copy everything
COPY Client ./
# Restore as distinct layers
RUN npm install

ENTRYPOINT ["npm", "start"]
EXPOSE 3000