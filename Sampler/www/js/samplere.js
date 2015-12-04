/**
 * Created by Ludo on 04/12/2015.
 */
'use strict';

var IP_ADDRESS_SAMPLER = '192.168.0.1';

function callSound(soundId) {
  $.post('/api/sound/'+soundId, function(data) {
     console.log(data);
  });
}