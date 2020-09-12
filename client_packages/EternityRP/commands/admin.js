mp.events.add ("playerCommand", (command) => {
		const args = command.split(" ");
		const commandName = args [0];
		mp.gui.chat.push (args.toString());
		args.shift ();
	  if (commandName === "vehicle") {
				mp.gui.chat.push ('хуй моржовый02'+args.toString());
	      switch(args[0]) {
	          case "create":
								mp.gui.chat.push ('хуй моржовый0'+args.toString());
	              if (args.length == 4){
	                  mp.events.callRemote('Command|CreateVehicle', args[1],args[2], args[3]);
										mp.gui.chat.push ('хуй моржовый1');
	              }else if (args.length == 2){
	                  mp.events.callRemote('Command|CreateVehicle', args[1], spawnCar, spawnCar);
										  mp.gui.chat.push ('хуй моржовый2');
	              } else {
	                  mp.gui.chat.push ('Используйте: /vehicle create model color1 color2 | /vehicle create model');
	              }
	              break;
	          case "repair":
	              mp.events.callRemote('Command|RepairVehicle', command);
	              break;
	          case "remove":
	              mp.events.callRemote('Command|RemoveVehicle', command);
	              break;

	          default:
	              mp.gui.chat.push ('Используйте: /vehicle create|remove|repair');

	      }
	  }
	  if (commandName === "getCoord") {
	      mp.events.callRemote('Command|GetСoord');
	  }
	  if (commandName === "a") {
	      if (args.lentgth !== ''){
	          mp.events.callRemote('Command|CreateVehicle', args[1]);
	      }
	  }

});
