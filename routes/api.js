const { Router } = require("express");
const getBalance = require("../controllers/api/getBalance");
const createAccount = require("../controllers/api/createAccount");
const createTransaction = require("../controllers/api/createTransaction");
const calculateCommission = require("../controllers/api/calculateCommission");

const router = Router();

router.get("/balance", getBalance);

router.get("/commission", calculateCommission);

router.post("/createaccount", createAccount);

router.post("/createtransaction", createTransaction);

module.exports = router;
