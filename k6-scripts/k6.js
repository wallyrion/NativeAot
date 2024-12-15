import { sleep, check } from 'k6';
import http from 'k6/http';
import { randomIntBetween } from 'https://jslib.k6.io/k6-utils/1.2.0/index.js';

export const options = {
    stages: [
        { duration: '30s', target: 5 },
         { duration: '1m', target: 1000 }, // simulate ramp-up of traffic from 1 to 100 users over 5 minutes.
        { duration: '1m', target: 0 }, // ramp-down to 0 users
        { duration: '10m', target: 5000 }, 
        { duration: '1m', target: 0 }
    ],
    thresholds: {
        http_req_failed: ['rate<0.01'], // http errors should be less than 1%
        http_req_duration: ['p(99)<300'], // 99% of requests must complete below 1.5s
    },
};

export function setup() {

}


export default function () {

    const query = `${__ENV.BASE_URL}/todos`;
    const response = http.get(query);
        /*['GET', 'http://104.46.60.108:3680/todos', null, { tags: { deployed: 'virtual_machine' } }],*/


    sleep(1)
    check(response, {
        'status 200 local': (res) => res.status === 200,
    });  
    
    
}