use std::fs;

#[derive(Clone, Debug)]
struct Point {
    x: isize,
    y: isize,
    has_roll: bool,
}

fn read_points_from_file(file_path: &str) -> Vec<Point> {
    let contents = fs::read_to_string(file_path).expect("Should have been able to read the file");
    let mut points: Vec<Point> = vec![];
    for (row, line) in contents.lines().enumerate() {
        for (col, c) in line.chars().enumerate() {
            points.push(Point {
                x: col as isize,
                y: row as isize,
                has_roll: c == '@',
            })
        }
    }

    points
}

fn main() {
    let file_path = "input.txt";
    let mut points = read_points_from_file(file_path);

    println!("Part 2 Result: {}", part2(&mut points));
}

fn part1(points: Vec<Point>) -> u32 {
    let mut accessible_rolls = 0u32;
    for point in &points {
        if !point.has_roll {
            continue;
        };

        let mut adjacent_rolls = 0;
        let adjacent_points: Vec<_> = points
            .iter()
            .filter(|other| {
                (point.x - 1..point.x + 2).contains(&other.x)
                    && (point.y - 1..point.y + 2).contains(&other.y)
                    && !(point.x == other.x && point.y == other.y)
            })
            .collect();
        for adjacent_point in adjacent_points {
            if adjacent_point.has_roll {
                adjacent_rolls += 1;
            }
        }
        if adjacent_rolls < 4 {
            accessible_rolls += 1;
        }
    }

    accessible_rolls
}

fn part2(points: &mut Vec<Point>) -> u32 {
    let mut sum = 0u32;
    loop {
        let mut accessible_rolls = 0u32;
        let points_clone = points.clone();
        for point in &mut *points {
            if !point.has_roll {
                continue;
            };

            let mut adjacent_rolls = 0;
            let adjacent_points: Vec<_> = points_clone
                .iter()
                .filter(|other| {
                    (point.x - 1..point.x + 2).contains(&other.x)
                        && (point.y - 1..point.y + 2).contains(&other.y)
                        && !(point.x == other.x && point.y == other.y)
                })
                .collect();
            for adjacent_point in adjacent_points {
                if adjacent_point.has_roll {
                    adjacent_rolls += 1;
                }
            }
            if adjacent_rolls < 4 {
                point.has_roll = false;
                accessible_rolls += 1;
            }
        }

        sum += accessible_rolls;
        if accessible_rolls == 0 {
            break;
        }
        println!("Accessible rolls this round: {}", accessible_rolls);
    }
    sum
}