const {web3Handler} = require("../../utils/web3Handler");

const createAccount = async (req, res, next) => {
  try {
    const web3 = await web3Handler.getWeb3();
    const account = web3.eth.accounts.create()
    res.status(200).send(account)
  } catch (err) {
    next(err)
  }
}

module.exports = createAccount;