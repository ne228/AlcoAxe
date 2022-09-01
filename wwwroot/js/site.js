

document.querySelector(".show").onclick = function () {


};

function loadedCards(el, count, gameid) {
    deleteCards();
    /* var data = `GameId=${gameid}&questionId=${questionId}`;*/
    $.ajax({
        type: 'GET',
        url: `/Games/Play?id=${gameid}&count=${count}&ajax=true`,
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        /* data: data,*/
        success: function (result) {
            console.log("ok play get");
            el.innerHTML = `<p>${result}<\p>`;
        },
        error: function () {
            alert('Failed to receive the Data');
            console.log('Failed ');
        }
    })
}


function deleteCards() {
    var tinder = document.querySelector(".tinder-container");
    tinder.parentNode.removeChild(tinder);

}