mergeInto(LibraryManager.library, {
	
  Save: function (data) {
	var dataString = UTF8ToString(data);
	var myObj = JSON.parse(dataString);
	player.setData(myObj);
  },
  
  Load: function () {
	player.getData().then(_date => {
		console.log("1");
		const myJSON = JSON.stringify(_date);
		console.log("2");
		myGameInstance.SendMessage('YandexManager', 'IsLoaded', myJSON);
		console.log("3");
		});
  },
  
});