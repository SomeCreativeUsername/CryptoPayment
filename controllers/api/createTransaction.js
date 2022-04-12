// noinspection JSUnusedLocalSymbols
const Web3 = require("web3");

const {web3Handler} = require("../../utils/web3Handler");

/** @typedef {Object} TransactionRequest
 * @property {String} amountTo
 * @property {String} addressTo
 * @property {String} privateKeyFrom
 * @property {String} gas
 * */

/**
 * @param {Object} req
 * @param {TransactionRequest} req.query
 * @param res
 * @param next
 * @return {Promise<void>}
 */
const createTransaction = async (req, res, next) => {
  try {
    const web3 = await web3Handler.getWeb3();
    const chainId = process.env.CHAIN_ID; // 97 is BSC testnet, 95 is BSC mainnet

    const amountTo = req.query.amountTo;//"0.01"; // ether

    const rawTx = {
      to: req.query.addressTo,
      value: web3.utils.toWei(amountTo, "ether"),
      chainId: chainId,
      gas: Number(web3.utils.toWei(req.query.gas))
    };

    createSignedTx(web3, req.query.privateKeyFrom, rawTx)
      .then((hash) => res.status(200).send({ hash }))
      .catch((err) => {
        console.log(err.message || "Unpredictable error");
        console.log(err);
        res.status(400).send(err);
      });
  } catch (err) {
    next(err);
  }
};

/**
 * @param {Web3} web3
 * @param {String} privateKeyFrom
 * @param {*} rawTx
 * @return {Promise<String>} - transaction hash
 */
const createSignedTx = (web3, privateKeyFrom, rawTx) =>
  new Promise(async (resolve, reject) => {
      console.log(rawTx);
    try {
      const signedTx = await signTransaction(web3, privateKeyFrom, rawTx);
      const result = await sendSignedTx(web3, signedTx);
      resolve(result);
    } catch (err) {
      if (err.err) reject(err);
      else reject({ err });
    }
  });


/**
 * @param {Web3} web3
 * @param {String} privateKeyFrom
 * @param {*} rawTx
 * @return {Promise<Transaction>} - estimated gas amount
 */
const signTransaction = (web3, privateKeyFrom, rawTx) =>
  new Promise((resolve, reject) => {
    const accountFrom = web3.eth.accounts.privateKeyToAccount(privateKeyFrom);
    accountFrom.signTransaction(rawTx, (err, signedTx) => {
      if (err) reject({ message: "Can't sign a transaction", err });
      else resolve(signedTx);
    });
  });

/**
 * @param {Web3} web3
 * @param {*} signedTx
 * @return {Promise<String>} - transaction hash
 */
const sendSignedTx = (web3, signedTx) =>
  new Promise((resolve, reject) => {
    web3.eth.sendSignedTransaction(signedTx.rawTransaction, (err, hash) => {
      if (err) reject({ message: "Can't send a signed transaction", err });
      else resolve(hash);
    });
  });

module.exports = createTransaction;
