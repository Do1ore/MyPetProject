// Create the scene
const scene = new THREE.Scene();

// Create the camera
const camera = new THREE.PerspectiveCamera(75, window.innerWidth / window.innerHeight, 0.1, 1000);
camera.position.set(0, 0, 5);

// Create the renderer
const renderer = new THREE.WebGLRenderer({ canvas: document.querySelector('#myCanvas') });
renderer.setSize(window.innerWidth, window.innerHeight);

// Add lighting to the scene
const ambientLight = new THREE.AmbientLight(0xffffff, 0.5);
scene.add(ambientLight);

const directionalLight = new THREE.DirectionalLight(0xffffff, 0.5);
directionalLight.position.set(0, 1, 1).normalize();
scene.add(directionalLight);
const loader = new THREE.GLTFLoader();
loader.load('C:\Users\vladx\source\repos\MyPet\MyPet\wwwroot\3d\SuperHand.glb', function (gltf) {
    const model = gltf.scene;
    scene.add(model);
});
function animate() {
    requestAnimationFrame(animate);
    // Rotate the model
    model.rotation.y += 0.01;
    // Render the scene
    renderer.render(scene, camera);
}
animate();
