require("dotenv").config();
const expressApp = require("./app");
const http = require("http");
const {web3Handler} = require("./utils/web3Handler");

async function start(app) {
  try {
    //enabling web3
    web3Handler.enableWeb3();
    console.log("Web3 enabled");
    //creating http or https server
    const server = http.createServer(app)
    //enabling listening
    server.listen(process.env.PORT, () => {
      console.log(
        `Server has been started on port ${process.env.PORT}`
      );
    });
  } catch (e) {
    console.error(e.message);
    process.exit(1);
  }
}

// noinspection JSIgnoredPromiseFromCall
start(expressApp);