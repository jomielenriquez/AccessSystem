const video = document.getElementById("video");

let modelsUrl = "https://raw.githubusercontent.com/justadudewhohacks/face-api.js/master/weights/";
//let modelsUrl = "/Scripts/model/";
Promise.all([
    faceapi.nets.ssdMobilenetv1.loadFromUri(modelsUrl + 'ssd_mobilenetv1_model-weights_manifest.json'),
    faceapi.nets.faceRecognitionNet.loadFromUri(modelsUrl + 'face_recognition_model-weights_manifest.json'),
    faceapi.nets.faceLandmark68Net.loadFromUri(modelsUrl + 'face_landmark_68_model-weights_manifest.json'),
    faceapi.nets.tinyFaceDetector.loadFromUri(modelsUrl + 'tiny_face_detector_model-weights_manifest.json'),

    //faceapi.nets.ssdMobilenetv1.loadFromUri("/Scripts/model"),
    //faceapi.nets.faceRecognitionNet.loadFromUri("/Scripts/model"),
    //faceapi.nets.faceLandmark68Net.loadFromUri("/Scripts/model"),
]).then(startWebcam);

function startWebcam() {
    navigator.mediaDevices
        .getUserMedia({
            video: true,
            audio: false,
        })
        .then((stream) => {
            video.srcObject = stream;
        })
        .catch((error) => {
            console.error(error);
        });
}

function getLabeledFaceDescriptions() {
    //const labels = ["sheldon", "Marus","4febeaf1-3553-4f18-839a-8f533fc91951"];
    const labels = listOfAllUsers;
    return Promise.all(
        labels.map(async (label) => {
            const descriptions = [];
            for (let i = 1; i <= 1; i++) {
                const img = await faceapi.fetchImage(`../Scripts/label/${label["NAMEID"]}/${i}.jpg`);
                const detections = await faceapi
                    .detectSingleFace(img)
                    .withFaceLandmarks()
                    .withFaceDescriptor();
                descriptions.push(detections.descriptor);
            }
            return new faceapi.LabeledFaceDescriptors(label["NAMEID"], descriptions);
        })
    );
}

video.addEventListener("play", async () => {
    const labeledFaceDescriptors = await getLabeledFaceDescriptions();
    const faceMatcher = new faceapi.FaceMatcher(labeledFaceDescriptors);
    const canvas = faceapi.createCanvasFromMedia(video);
    document.body.getElementsByTagName("main")[0].getElementsByTagName("div")[0].getElementsByTagName("div")[0].append(canvas);

    const displaySize = { width: video.width, height: video.height };
    faceapi.matchDimensions(canvas, displaySize);

    setInterval(async () => {
        const detections = await faceapi
            .detectAllFaces(video)
            .withFaceLandmarks()
            .withFaceDescriptors();

        const resizedDetections = faceapi.resizeResults(detections, displaySize);

        canvas.getContext("2d").clearRect(0, 0, canvas.width, canvas.height);

        const results = resizedDetections.map((d) => {
            return faceMatcher.findBestMatch(d.descriptor);
        });
        results.forEach((result, i) => {
            if (i == 0) { // Limiting the detection to one face only
                const box = resizedDetections[i].detection.box;
                var userID = result._label;
                var info = listOfAllUsers.filter(function (n, i) { return n.NAMEID == result._label })
                try {
                    result._label = info[0]["FIRSTNAME"] + " " + info[0]["LASTNAME"];
                }
                catch {
                    result._label = "UNKNOWN";
                }

                const drawBox = new faceapi.draw.DrawBox(box, {
                    label: result,
                });
                //console.log(result._label);
                $("#staticName").val(result._label);
                $("#staticUserID").val(userID);
                RFIDandName_Changed();
                drawBox.draw(canvas);
            }
            if (i > 0) {
                console.log("error");
            }
        });
    }, 500);
});
