// random Numbers
const random = () => Math.round(Math.random() * 100);

var nameCt = 11;

function ChartData(cityName,
    timeRange1, timeRange2, timeRange3, timeRange4, timeRange5, timeRange6, timeRange7,
    tempData1, tempData2, tempData3, tempData4, tempData5, tempData6, tempData7,
    cloudsData1, cloudsData2, cloudsData3, cloudsData4, cloudsData5, cloudsData6, cloudsData7,
    pressureData1, pressureData2, pressureData3, pressureData4, pressureData5, pressureData6, pressureData7,
    humidity) {

    // eslint-disable-next-line no-unused-vars
    const lineChart = new Chart(document.getElementById('canvas-1'), {
        type: 'line',
        data: {
            labels: [timeRange1, timeRange2, timeRange3, timeRange4, timeRange5, timeRange6, timeRange7],
            datasets: [{
                label: cityName,
                backgroundColor: 'rgba(64, 45, 170, 0.2)',
                borderColor: 'rgba(64, 45, 170, 1)',
                pointBackgroundColor: 'rgba(64, 45, 170, 1)',
                pointBorderColor: '#fff',
                data: [tempData1, tempData2, tempData3, tempData4, tempData5, tempData6, tempData7]
            }]
        },
        options: {
            responsive: true
        }
    });

    // eslint-disable-next-line no-unused-vars
    const barChart = new Chart(document.getElementById('canvas-2'), {
        type: 'bar',
        data: {
            labels: [timeRange1, timeRange2, timeRange3, timeRange4, timeRange5, timeRange6, timeRange7],
            datasets: [{
                label: cityName,
                backgroundColor: 'rgba(143, 193, 246, 0.5)',
                borderColor: 'rgba(143, 193, 246, 0.8)',
                highlightFill: 'rgba(143, 193, 246, 0.75)',
                highlightStroke: 'rgba(143, 193, 246, 1)',
                data: [cloudsData1, cloudsData2, cloudsData3, cloudsData4, cloudsData5, cloudsData6, cloudsData7]
            }]
        },
        options: {
            responsive: true
        }
    });

    // eslint-disable-next-line no-unused-vars
    const doughnutChart = new Chart(document.getElementById('canvas-3'), {
        type: 'doughnut',
        data: {
            labels: ['Humidity', '%'],
            datasets: [{
                data: [humidity, 100 - humidity],
                backgroundColor: ['#402DAA', '#4d4d4d'],
                hoverBackgroundColor: ['#402DAA', '#4d4d4d']
            }]
        },
        options: {
            responsive: true
        }
    });

    // eslint-disable-next-line no-unused-vars
    const linePressureChart = new Chart(document.getElementById('canvas-10'), {
        type: 'line',
        data: {
            labels: [timeRange1, timeRange2, timeRange3, timeRange4, timeRange5, timeRange6, timeRange7],
            datasets: [{
                label: cityName,
                backgroundColor: 'rgba(37, 97, 221, 0.2)',
                borderColor: 'rgba(37, 97, 221, 1)',
                pointBackgroundColor: 'rgba(37, 97, 221, 1)',
                pointBorderColor: '#fff',
                data: [pressureData1, pressureData2, pressureData3, pressureData4, pressureData5, pressureData6, pressureData7]
            }]
        },
        options: {
            responsive: true
        }
    });
}



// eslint-disable-next-line no-unused-vars
const radarChart = new Chart(document.getElementById('canvas-4'), {
  type: 'radar',
  data: {
    labels: ['Eating', 'Drinking', 'Sleeping', 'Designing', 'Coding', 'Cycling', 'Running'],
    datasets: [{
      label: 'My First dataset',
      backgroundColor: 'rgba(220, 220, 220, 0.2)',
      borderColor: 'rgba(220, 220, 220, 1)',
      pointBackgroundColor: 'rgba(220, 220, 220, 1)',
      pointBorderColor: '#fff',
      pointHighlightFill: '#fff',
      pointHighlightStroke: 'rgba(220, 220, 220, 1)',
      data: [65, 59, 90, 81, 56, 55, 40]
    }, {
      label: 'My Second dataset',
      backgroundColor: 'rgba(151, 187, 205, 0.2)',
      borderColor: 'rgba(151, 187, 205, 1)',
      pointBackgroundColor: 'rgba(151, 187, 205, 1)',
      pointBorderColor: '#fff',
      pointHighlightFill: '#fff',
      pointHighlightStroke: 'rgba(151, 187, 205, 1)',
      data: [28, 48, 40, 19, 96, 27, 100]
    }]
  },
  options: {
    responsive: true
  }
});

// eslint-disable-next-line no-unused-vars
const pieChart = new Chart(document.getElementById('canvas-5'), {
  type: 'pie',
  data: {
    labels: ['Red', 'Green', 'Yellow'],
    datasets: [{
      data: [300, 50, 100],
      backgroundColor: ['#FF6384', '#36A2EB', '#FFCE56'],
      hoverBackgroundColor: ['#FF6384', '#36A2EB', '#FFCE56']
    }]
  },
  options: {
    responsive: true
  }
});

// eslint-disable-next-line no-unused-vars
const polarAreaChart = new Chart(document.getElementById('canvas-6'), {
  type: 'polarArea',
  data: {
    labels: ['Red', 'Green', 'Yellow', 'Grey', 'Blue'],
    datasets: [{
      data: [11, 16, 7, 3, 14],
      backgroundColor: ['#FF6384', '#4BC0C0', '#FFCE56', '#E7E9ED', '#36A2EB']
    }]
  },
  options: {
    responsive: true
  }
});
//# sourceMappingURL=charts.js.map