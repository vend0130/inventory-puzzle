mergeInto(LibraryManager.library, {
	
  Save: function (data) {
	if(player == null){
	    console.log("FAIL SAVE DATA");
		return;
	}
	
	var dataString = UTF8ToString(data);
	var myObj = JSON.parse(dataString);
	console.log("DATA IS SAVED");
	player.setData(myObj);
  },
  
  Load: function () {
	if(player == null){
		myGameInstance.SendMessage('YandexManager', 'IsFailLoaded');
		console.log("FAIL LOAD DATA");
		return;
	}
	  
	player.getData().then(_date => {
		const myJSON = JSON.stringify(_date);
		console.log("DATA IS LOADED");
		myGameInstance.SendMessage('YandexManager', 'IsLoaded', myJSON);
		});
  },
  
});