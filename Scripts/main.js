/*
Copyright 2017 Google Inc.
Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at
    http://www.apache.org/licenses/LICENSE-2.0
Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

'use strict';

var videoElement = document.querySelector('video');
var videoSelect = document.querySelector('select#videoSource');
const video = document.getElementById('video');
const canvas = document.getElementById('canvas');
var photo = document.querySelector('#photo');
const PrendreButtonPhoto = document.getElementById('PrendreButtonPhoto');
var width = 260;
var height = 0;
var streaming = false;

video.addEventListener('canplay', function (ev) {
    if (!streaming) {
        height = video.videoHeight / (video.videoWidth / width);
        video.setAttribute('width', width);
        video.setAttribute('height', height);
        canvas.setAttribute('width', width);
        canvas.setAttribute('height', height);
        console.log(width + '-' + height);
        streaming = true;
    }
}, false);

try {
    PrendreButtonPhoto.onclick = () => {
        canvas.width = width;
        canvas.height = height;
        photo.width = width;
        photo.height = height;
        console.log(width + '-' + height);
    canvas.getContext('2d').drawImage(video, 0, 0, width, width);
    var data = canvas.toDataURL('image/png');
    photo.setAttribute('src', data);
}
    
    videoSelect.onchange = getStream;

    getStream().then(getDevices).then(gotDevices);
} catch (err) {
    console.error(err);
}

function getDevices() {
    // AFAICT in Safari this only gets default devices until gUM is called :/
    return navigator.mediaDevices.enumerateDevices();
}

function gotDevices(deviceInfos) {
    window.deviceInfos = deviceInfos; // make available to console
    console.log('Available input and output devices:', deviceInfos);
    for (const deviceInfo of deviceInfos) {
        const option = document.createElement('option');
        option.value = deviceInfo.deviceId;
         if (deviceInfo.kind === 'videoinput') {
            option.text = deviceInfo.label || `Camera ${videoSelect.length + 1}`;
            videoSelect.appendChild(option);
        }
    }
}

function getStream() {
    if (window.stream) {
        window.stream.getTracks().forEach(track => {
            track.stop();
        });
    }
   
    const videoSource = videoSelect.value;
    const constraints = {

        audio: false,
        video: {
            deviceId: videoSource ? { exact: videoSource } : undefined  }
    };
    return navigator.mediaDevices.getUserMedia(constraints).
        then(gotStream).catch(handleError);
}

function gotStream(stream) {
    window.stream = stream; // make stream available to console
     videoSelect.selectedIndex = [...videoSelect.options].
        findIndex(option => option.text === stream.getVideoTracks()[0].label);
    videoElement.srcObject = stream;
}

function handleError(error) {
    console.error('Error: ', error);
}