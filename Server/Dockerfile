# Semcomp 24 - Game Server
# sudo docker build -t semcomp24-game:1.0 .
# sudo docker run -d -p 3000:3000 --rm --name semcomp24-game-inst semcomp24-game:1.0
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

# Expose HTTP/HTTPS Default port
EXPOSE 3000

# Start node
CMD [ "node", "./src/index.js" ]