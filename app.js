// noinspection JSUnresolvedFunction

const express = require("express");
const Router = require("./routes/api");

const app = express();

app.use(express.urlencoded({ extended: true }));
app.use(express.json());

app.use("/api", Router);

// noinspection JSUnusedLocalSymbols
app.use((err, req, res, next) => {
  console.error(err);
  if (err?.status) return res.status(err.status).send(err);
  res.status(500).send({
    error: err?.message || null,
    request: req?.url || null,
    message: "Something went wrong :c",
  });
});

module.exports = app;
