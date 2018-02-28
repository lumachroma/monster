var config = {
    apiKey: "AIzaSyAztYFDtQMHxrpauotuYMqzHgZQCfZqKd8",
    authDomain: "swag-monster.firebaseapp.com",
    databaseURL: "https://swag-monster.firebaseio.com",
    projectId: "swag-monster",
};
try {
    var monsterApp = firebase.app("monster");
} catch (e) {
    var monsterApp = firebase.initializeApp(config, "monster");
}
var usersRef = monsterApp.database().ref("/Users");
var auctionsRef = monsterApp.database().ref("/Auctions");