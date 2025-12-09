use std::fs;

fn main() {
    let file_path = "input.txt";
    let ranges = read_ranges_from_file(file_path);
    let sum = part2(ranges);
    println!("Sum: {}", sum);
}

fn part1(ranges: Vec<(i64, i64)>) -> i64 {
    let mut sum = 0;
    for (min, max) in ranges {
        for num in min..=max {
            println!("{}", num);
            if check_repetition(num) {
                println!("Found repetition: {}", num);
                sum += num;
            }
        }
    }
    sum
}

fn part2(ranges: Vec<(i64, i64)>) -> i64 {
    let mut sum = 0;
    for (min, max) in ranges {
        for num in min..=max {
            println!("{}", num);
            if check_multiple_repetition(num) {
                println!("Found repetition: {}", num);
                sum += num;
            }
        }
    }
    sum
}

fn read_ranges_from_file(file_path: &str) -> Vec<(i64, i64)> {
    let contents = fs::read_to_string(file_path)
        .expect("Should have been able to read the file");
    contents.split(",").map(|range| {
        let bounds: Vec<&str> = range.trim().split("-").collect();
        let start: i64 = bounds[0].parse().unwrap();
        let end: i64 = bounds[1].parse().unwrap();
        (start, end)
    }).collect()
}

fn check_repetition(num: i64) -> bool {
    let num_str = num.to_string();
    let chars: Vec<char> = num_str.chars().collect();
    let first_half = &chars[..chars.len() / 2];
    let second_half = &chars[chars.len() / 2..];
    first_half == second_half
}

fn check_multiple_repetition(num: i64) -> bool {
    let num_str = num.to_string();
    let chars: Vec<char> = num_str.chars().collect();
    // Determine how many equally sized parts we can split the number into
    let len = chars.len();
    let mut has_repetition = false;
    for part_size in 1..=len / 2 {
        if len % part_size != 0 {
            continue;
        }
        // Make the parts
        let mut parts: Vec<&[char]> = Vec::new();
        for i in (0..len).step_by(part_size) {
            parts.push(&chars[i..i + part_size]);
        };
        // Check if all parts are the same
        let first_part = parts[0];
        if parts.iter().all(|part| *part == first_part) {
            has_repetition = true;
        }
    }
    has_repetition
}