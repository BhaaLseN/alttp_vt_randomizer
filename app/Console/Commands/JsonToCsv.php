<?php

namespace ALttP\Console\Commands;

use Illuminate\Console\Command;
use ALttP\Item;
use ALttP\Randomizer;
use ALttP\World;

/**
 * This converts json files to csv.
 */
class JsonToCsv extends Command
{
    /**
     * The name and signature of the console command.
     *
     * @var string
     */
    protected $signature = 'alttp:jsontocsv {file}';

    /**
     * The console command description.
     *
     * @var string
     */
    protected $description = 'convert json file to csv.';

    /**
     * Execute the console command.
     *
     * @return mixed
     */
    public function handle()
    {
        if (!is_string($this->argument('file')) || !is_readable($this->argument('file'))) {
            $this->error("json not readable");

            return 1;
        }

        $file_contents = file_get_contents($this->argument('file'));

        if ($file_contents === false) {
            $this->error('could not read file');

            return 3;
        }

        $locations = $this->_assureColumnsExist(json_decode($file_contents, true));
        ksortr($locations);
        $out = fopen('php://output', 'w');

        if ($out === false) {
            $this->error('could not open output');

            return 2;
        }

        fputcsv($out, array_merge(['location'], array_keys(reset($locations))));
        foreach ($locations as $name => $location) {
            fputcsv($out, array_merge([$name], $location));
        }
        fclose($out);
    }

    private function _assureColumnsExist($array): array
    {
        $keys = [];
        foreach ($array as $part) {
            $keys = array_merge($keys, array_keys($part));
        }
        $keys = array_unique($keys);
        foreach ($array as $k => $part) {
            foreach ($keys as $key) {
                if (!isset($part[$key])) {
                    $array[$k][$key] = 0;
                }
            }
        }
        return $array;
    }
}
