const random = () => Math.round(Math.random() * 100);

// eslint-disable-next-line no-unused-vars
const lineChart = new Chart(document.getElementById('canvas-1'), {
    type: 'line',
    data: {
        labels: ['January', 'February', 'March', 'April', 'May', 'June', 'July'],
        datasets: [{
            label: 'My First dataset',
            backgroundColor: 'rgba(220, 220, 220, 0.2)',
            borderColor: 'rgba(220, 220, 220, 1)',
            pointBackgroundColor: 'rgba(220, 220, 220, 1)',
            pointBorderColor: '#fff',
            data: [random(), random(), random(), random(), random(), random(), random()]
        }, {
            label: 'My Second dataset',
            backgroundColor: 'rgba(151, 187, 205, 0.2)',
            borderColor: 'rgba(151, 187, 205, 1)',
            pointBackgroundColor: 'rgba(151, 187, 205, 1)',
            pointBorderColor: '#fff',
            data: [random(), random(), random(), random(), random(), random(), random()]
        }]
    },
    options: {
        responsive: true
    }
});