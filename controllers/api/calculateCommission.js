// noinspection JSUnusedLocalSymbols
const Web3 = require("web3");
const {web3Handler} = require("../../utils/web3Handler");

/** @typedef {Object} ComissionRequest
 * @property {String} amountTo
 * @property {String} addressTo
 * */

/**
 * @param {Object} req
 * @param {ComissionRequest} req.query
 * @param res
 * @param next
 * @return {Promise<void>}
 */
const calculateCommission = async (req, res, next) => {
  try {
    const web3 = await web3Handler.getWeb3();
    const chainId = process.env.CHAIN_ID;

    const rawTx = {
      to: req.query.addressTo,
      value: web3.utils.toWei(req.query.amountTo, "ether"),
      chainId: chainId,
    };

    const rawTxComsa = {
      to: process.env.COMISSION_WALLET_ADDRESS,
      value: web3.utils.toWei(String(req.query.amountTo * 0.015), "ether"),
      chainId: chainId,
    };

    const commGas = await estimateGas(web3, rawTxComsa);

    const mainGas = await estimateGas(web3, rawTx);

    const comissionGas = web3.utils.fromWei(commGas.toString(), 'ether');

    const amountGas = web3.utils.fromWei(mainGas.toString(), 'ether');

    res.status(200).send({
      inputAmount: req.query.amountTo,
      comission: req.query.amountTo * 0.015,
      comissionGas,//: web3.utils.fromWei(mainGas, 'ether'),
      amountGas,//: web3.utils.fromWei(commissionGas, 'ether'),
    })
  } catch (err) {
    next(err);
  }
}

/**
 * @param {Web3} web3
 * @param {*} rawTx
 * @return {Promise<number>} - estimated gas amount
 */
const estimateGas = (web3, rawTx) =>
  new Promise((resolve, reject) => {
    web3.eth.estimateGas(rawTx, (err, gas) => {
      if (err) reject({ message: "Can't estimate gas", err });
      else resolve(gas);
    });
  });


module.exports = calculateCommission;