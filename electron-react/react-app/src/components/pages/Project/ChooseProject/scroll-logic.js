var $ = require("jquery");
var DataTable = require("datatables.net-dt");

$(document).ready(function () {
  $("#dtVerticalScrollExample").DataTable({
    scrollY: "200px",
    scrollCollapse: true,
  });
  $(".dataTables_length").addClass("bs-select");
  $("#dtVerticalScrollExample_length").css("display", "none");
  $("#dtVerticalScrollExample_filter").css("display", "none");
  $("#dtVerticalScrollExample_info").css("display", "none");
  $("#dtVerticalScrollExample_paginate").css("display", "none");
});
