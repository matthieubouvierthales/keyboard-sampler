/**
 * Created by Ludo on 04/12/2015.
 */
'use strict';

responsiveVoice.setDefaultVoice("French Female");

function callSound(soundId) {
  $.post('/api/sound/'+soundId, function(data) {
     console.log(data);
  });
}