$(document).ready(function () {
    var url = 'http://localhost:5545/';
    
   
    var renderBooks = function (result1) {
        $("#booklist").empty();
        result1.forEach(function (b) {
            var title = $("<div>", { class: "element" }).append(b.Title);
            var author = $("<div>", { class: "element" }).append(b.Author);
            var listElement = $("<div>", { class: "entry" }).append(title).append(author);           
            $("#booklist").append(listElement);
        }); 
    };

    $.ajax({
        url: url + '/api/book/catalog', success: function (result) {
            console.log("ajax along is called");
            renderBooks(result);
        }, error: function (request, error) {
            alert(request.responseJSON.Message);
        }
    });

    $("#addabook")
        .click(function() {
            $("#catalog").hide();
            $(".entry").find("input:text").val("");
            $("#addone").show();
        });

    $("#deleteabook")
       .click(function () {
           $("#catalog").hide();
           $("#addone").find("input:text").val("");
           $("#addone").show();
       });

    $("#editabook")
       .click(function () {
           $("#catalog").hide();
           $(".entry").find("input:text").val("");
           $("#eidtone").show();
       });

    $("#addOk")
        .click(function () {           
            if ((!$("#title").val()) || (!$("#author").val())) { alert("please enter the title and author name"); return false; }
            $.ajax({
                url: url + "/api/book/add",
                type: "POST",
                data: JSON.stringify({ title: $("#title").val(), author: $("#author").val() }),
                success: function (result) {
                    
                    renderBooks(result);
                }, error: function (request, error) {
                    alert(request.responseJSON.Message);
                }
            });
            $("#addone").hide();
            $("#catalog").show();
        });

    $("#editOk")
      .click(function () {
          if (!$("#tit").val() || !$("#aut").val()|| !$("#price").val()||
              !$("#date").val()|| !$("#genre").val()||!$("#description").val())
          {
              //console.log();
              alert("all fields required"); return false;
          }
          $.ajax({
              url: url + "/api/book/edit",
              type: "POST",
              data: JSON.stringify({ title: $("#tit").val(), author: $("#aut").val(),price: $("#price").val(),
                  date: $("#date").val(), genre: $("#genre").val(), description: $("#description").val()
              }),
              success: function (result) {
                  //console.log("success: " + result);
                  renderBooks(result);
              }, error: function (request, error) {
                  alert(request.responseJSON.Message);
              }
          });
          $("#eidtone").hide();
          $("#catalog").show();
      });

    $("#deleteOk")
        .click(function () {
            if ((!$("#title").val()) || (!$("#author").val())) { alert("please enter the title and author name"); return false; }
            $.ajax({
                url: url + "/api/book/delete",
                type: "POST",
                data: JSON.stringify({ title: $("#title").val(), author: $("#author").val() }),
                success: function (result) {
                    //console.log("success: " + result);
                    renderBooks(result);
                }, error: function (request, error) {
                    alert(request.responseJSON.Message);
                }
            });
            $("#addone").hide();
            $("#catalog").show();
        });

    $("#groupby")
       .click(function () {
           $.ajax({
               url: url + "/api/book/groupby",
               type: "POST",
               success: function (result) {
                  // console.log("success: " + result);
                   renderBooks(result);
               }, error: function (request, error) {
                   alert(request.responseJSON.Message);
               }
           });
           $("#addone").hide();
           $("#catalog").show();
       });

   
    $("#sortbooks")
       .click(function () {
           $.ajax({
               url: url + "/api/book/sort",
               type: "POST",
               success: function (result) {
                   console.log("success: " + result);
                   renderBooks(result);
               }, error: function (request, error) {
                   alert(request.responseJSON.Message);
               }
           });
           $("#addone").hide();
           $("#catalog").show();
       });   

});

