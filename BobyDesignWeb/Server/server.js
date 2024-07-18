console.log('NodeServer running');

const express = require('express');
const app = express();

app.use(express.static('public'));

app.set('view engine', 'ejs');

const fs = require('fs');
const path = require('path');

function findMatchingFile(dir, pattern) {
    const regex = new RegExp(pattern);
    const files = fs.readdirSync(dir);
    for (const file of files) {
      if (regex.test(file)) {
        return file;
      }
    }
    throw new Error('No matching file found');
  }

app.get('/', (req, res) => {
    const dir = path.join(__dirname, 'public', 'ngdist');
    const mainScript = path.join('ngdist', findMatchingFile(dir, '^main\\..*\\.js$'));
    const polyfillsScript = path.join('ngdist', findMatchingFile(dir, '^polyfills\\..*\\.js$'));
    const runtimeScript = path.join('ngdist', findMatchingFile(dir, '^runtime\\..*\\.js$'));
    const styles = path.join('ngdist', findMatchingFile(dir, '^styles\\..*\\.css$'));
    res.render('index', {mainScript, polyfillsScript, runtimeScript, styles});
})

app.listen(3000);