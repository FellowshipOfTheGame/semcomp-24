# Semcomp 24 - Game Server
# sudo docker build -t semcomp-24:1.0 .
# sudo docker run -d -p 3000:3000 --rm --name semcomp24-server semcomp-24:1.0
FROM node:15.14

# Create app directory
WORKDIR /usr/src/app

# Install app dependencies
# A wildcard is used to ensure both package.json AND package-lock.json are copied
# where available (npm@5+)
COPY package*.json ./

RUN npm install

# Bundle app source
COPY . .

# Expose HTTP and HTTPS Default
EXPOSE 3000
EXPOSE 3030

# Start node
CMD [ "node", "./src/index.js" ]