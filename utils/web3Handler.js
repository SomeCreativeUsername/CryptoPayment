const Web3 = require("web3");

class CustomWeb3 {
  /**@type {Web3} internalValue - DON'T ACCESS IT DIRECTLY*/
  internalWeb3 = null;
  reconnecting = false;
  web3Resolvers = [];

  enableWeb3 = () => {
    const type = process.env.NODE_TYPE;
    const wsUrl = process.env.WS_NODE_URL;
    const httpUrl = process.env.HTTP_NODE_URL;

    const getProvider = () => {
      let provider;
      if (type === "WS") {
        provider = new Web3.providers.WebsocketProvider(wsUrl);
        provider.on("end", () => {
          console.log("WS node has been disconnected, reconnecting");
          this.reconnect();
        });
      } else {
        provider = new Web3.providers.HttpProvider(httpUrl);
      }
      return provider;
    };

    try {
      const provider = getProvider();
      this.internalWeb3 = new Web3(provider);
      return this.internalWeb3;
    } catch (err) {
      console.log("Can't connect to web3 node");
      console.error(err);
      process.exit(1);
    }
  };

  reconnect = async (attempt = 0) => {
    try {
      this.reconnecting = true;
      const provider = new Web3.providers.WebsocketProvider(
        process.env.MORALIS_SPEEDY_NODE_WS_URL
      );
      provider.on("end", () => {
        console.log("WS node has been disconnected, reconnecting");
        this.reconnect();
      });
      this.internalWeb3.setProvider(provider);
      this.reconnecting = false;
      this.web3Resolvers.forEach((resolver) => resolver(this.internalWeb3));
      this.web3Resolvers = [];
    } catch (err) {
      if (attempt >= 3) {
        console.error("Reconnect to Web3 node failed");
        console.error(err);
        process.exit(1);
      } else {
        this.reconnect(attempt + 1).then();
      }
    }
  };

  /**@return {Promise<Web3>} web3 instance */
  getWeb3 = () => {
    let exec;
    const promise = new Promise((resolve) => {
      exec = resolve;
    });
    if (this.reconnecting) {
      this.web3Resolvers.push(exec);
    } else {
      exec(this.internalWeb3);
    }
    return promise;
  };
}

module.exports = { web3Handler: new CustomWeb3() };
