mp.events.add('Connection_Player', (SocialName, status) => {
    mp.gui.chat.push(SocialName);
      //mp.gui.cursor.show(true, true);

    // status
    // 0 - Забанен
    // 1 - Регистрация
    // 2 - Авторизация

    mp.gui.cursor.show(true, true);
    //mp.gui.cursor.visible = true; Почему так а не как выше
    mp.events.call(
        'createBrowser',
        'package://EternityRP/statics/html/login.html',
        'init',
        {
            socialName: SocialName,
            blocked: status === 0,
            go: status === 1? 'registration': status === 2? 'login': null
        }
    );





    if (status == 0){
    } else if (status == 1){
        mp.gui.cursor.show(true, true);
        mp.events.call('createBrowser', ['package://EternityRP/statics/html/register.html']);
        mp.events.call('executeFunction', ["socialClub"], SocialName);
        mp.events.add('registerInformationToServer', (/*login, */email, password) => {
          mp.events.callRemote('OnPlayerRegisterAttempt', "__Rant__"/*login*/, email, password);
          });
        mp.events.add('RegisterResultFalse', () => {
          mp.events.call('executeFunction', ['emailStatus']);
            });
        mp.events.add('RegisterResultTrue', () => {
              mp.events.call('destroyBrowser');
              mp.events.call('createBrowser', ['package://EternityRP/statics/html/login.html']);
            });
    }else{
        mp.gui.cursor.show(true, true);
        mp.events.call('createBrowser', ['package://EternityRP/statics/html/login.html']);
        mp.events.call('executeFunction', ['socialClub'], SocialName);
        mp.events.add('LoginResultFalse', () => {
          mp.events.call('executeFunction', ['errorPassLog']);
        });
        mp.events.add('LoginResultTrue', (bay_pers/*, donat, репорты, новости  и вообще вся инф о аккаунте.*/) => {
          mp.events.call('destroyBrowser');
          mp.events.call('createBrowser', ['package://EternityRP/statics/html/changePers.html']);  //Тебе нужно будт сделать, чтобы часто вызываемые окна скрывались, а не каждый раз их удалять и зохдавать.(чтобы они были в оперативе)
          //mp.events.call('executeFunction', ['socialClub'], SocialClubName); //   Чтобы можно было написать Social and Donat вместе
          //mp.events.call('executeFunction', ['donatFunk'], donat); //(нужно подумать как сделать добавление переменных в одной функцию)^
          mp.events.add('СPlayer1', (id, name, money, age, status) => {
              mp.events.call('executeFunction', ['Pers1Function', id, name, money, age]);
              mp.gui.chat.push(id.toString()+"клиент1");
              mp.gui.chat.push(name);
              mp.gui.chat.push(money.toString());
              mp.gui.chat.push(age.toString());
          });
          mp.events.add('СPlayer2', (id, name, money, age, status) => {
              mp.events.call('executeFunction', ['Pers2Function', id, name, money, age]);
              mp.gui.chat.push(id.toString()+"клиент2");
              mp.gui.chat.push(name);
              mp.gui.chat.push(money.toString());
              mp.gui.chat.push(age.toString());
          });
          mp.events.add('СPlayer3', (id, name, money, age, status) => {
              mp.events.call('executeFunction', ['Pers3Function', id, name, money, age]);
              mp.gui.chat.push(name);
              mp.gui.chat.push(money.toString());
              mp.gui.chat.push(age.toString());
          });
        });
   }
});
//CEF TO CLIENT
mp.events.add('OpenCreatePers', (/*number_pers  хз как это дальше использовать, нужно как-то запомнить какого персонажа он создает(1-о, 2-о или 3-о)*/) => {
  mp.events.call('createBrowser', ['package://EternityRP/statics/html/characterCreator.html']);  //Тебе нужно будт сделать, чтобы часто вызываемые окна скрывались, а не каждый раз их удалять и создавать.(чтобы они были в оперативе)
});
//END

//CLIENT TO CEF

//END

// CEF TO SERVER
mp.events.add('loginInformationToServer', (password) => {
    mp.events.callRemote('OnPlayerLoginAttempt', "__Rant__", password);
});
mp.events.add('ResetEmailInformationToServer', (email) => {
    mp.events.callRemote('ResetEmailToServer', email)
});
mp.events.add('ResetCodelInformationToServer', (code) => {
    mp.events.callRemote('ResetCodeToServer', code);
});
mp.events.add('newPassInformationToServer', (password) => {
  mp.events.callRemote('newPassToServer', password)
});
mp.events.add('PlayToClient', (id) => {
  mp.gui.chat.push(id.toString()+"На сервер");
  mp.events.callRemote('PlayToServer', id);
  mp.gui.cursor.show(false, false);
  mp.events.call('destroyBrowser');
});
// END

// SERVER TO CEF
mp.events.add('ReturnAnswer', (answer) => {
    if (answer == 0){
        resetBrowser.execute('var alertElement = $(\' <input type="text" id="codeBox" disabled placeholder="Неверный код!!!"> \'); \
            $(\'#loginText\').appent(alertElement);'); // В новый формат.
    } else{
      function newPassReqest() {
        newPass();
      }
  }
});
  // END
