/**
 * Created by Ludo on 04/12/2015.
 */
'use strict';

function callSound(soundId) {
  $.post('/api/sound/'+soundId, function(data) {
     console.log(data);
  });
}