const {web3Handler} = require("../../utils/web3Handler");

const getBalance = async (req, res, next) => {
  try {
    const hash = req.query.hash;
    const web3 = await web3Handler.getWeb3();
    await web3.eth.getBalance(hash, web3.eth.defaultBlock, (err, balance) => {
      if (err) return res.status(400).send(err)
      return res.status(200).send({balance})
    });
  } catch (err) {
    next(err)
  }
}

module.exports = getBalance;